// Caution! Be sure you understand the caveats before publishing an application with
// offline support. See https://aka.ms/blazor-offline-considerations

self.importScripts('./service-worker-assets.js');
self.addEventListener('install', event => event.waitUntil(onInstall(event)));
self.addEventListener('activate', event => event.waitUntil(onActivate(event)));
self.addEventListener('fetch', event => event.respondWith(onFetch(event)));

// A new version waits until the update prompt in reuniverse.js asks it to take over,
// as taking over right away would delete the cache the running version still loads from.
self.addEventListener('message', event => {
    if (event.data?.type === 'SKIP_WAITING') {
        self.skipWaiting();
    }
});

const cacheNamePrefix = 'offline-cache-';
const cacheName = `${cacheNamePrefix}${self.assetsManifest.version}`;
const offlineAssetsInclude = [ /\.dll$/, /\.pdb$/, /\.wasm/, /\.html/, /\.js$/, /\.json$/, /\.css$/, /\.woff$/, /\.png$/, /\.jpe?g$/, /\.gif$/, /\.ico$/, /\.blat$/, /\.dat$/, /\.webmanifest$/ ];
const offlineAssetsExclude = [ /^service-worker\.js$/ ];

// Replace with your base path if you are hosting on a subfolder. Ensure there is a trailing '/'.
const base = "/";
const baseUrl = new URL(base, self.origin);
const manifestUrlList = self.assetsManifest.assets.map(asset => new URL(asset.url, baseUrl).href);

async function onInstall(event) {
    console.info('Service worker: Install');

    // Fetch and cache all matching items from the assets manifest
    const assetsRequests = self.assetsManifest.assets
        .filter(asset => offlineAssetsInclude.some(pattern => pattern.test(asset.url)))
        .filter(asset => !offlineAssetsExclude.some(pattern => pattern.test(asset.url)))
        //.map(asset => new Request(asset.url, { integrity: asset.hash, cache: 'no-cache' }));
        .map(asset => {
            const isBrotliAsset = /\.(dll|wasm|pdb|dat)$/.test(asset.url);

            if (isBrotliAsset) {
                // Append .br to match what loadBootResource fetches.
                // Omit integrity — the hash is for the uncompressed file.
                return new Request(asset.url + '.br', { cache: 'no-cache' });
            } else {
                return new Request(asset.url, { integrity: asset.hash, cache: 'no-cache' });
            }
        });
    await caches.open(cacheName).then(cache => cache.addAll(assetsRequests));
}

async function onActivate(event) {
    console.info('Service worker: Activate');

    // Delete unused caches
    const cacheKeys = await caches.keys();
    await Promise.all(cacheKeys
        .filter(key => key.startsWith(cacheNamePrefix) && key !== cacheName)
        .map(key => caches.delete(key)));
}

async function onFetch(event) {
    if (event.request.method !== 'GET') {
        return;
    }

    const url = new URL(event.request.url);

    // Add any paths that should always be fetched from the network here
    const directHitPaths = [];
    const isDirectHit = directHitPaths.some(path => url.pathname.startsWith(base + path));

    if (isDirectHit) {
        return fetch(event.request);
    }

    const isNavigationRequest = event.request.mode === 'navigate';
    const hasFileExtension = url.pathname.match(/\.[a-zA-Z0-9]+$/);

    const shouldServeIndexHtml = isNavigationRequest
        && !hasFileExtension
        && !manifestUrlList.some(manifestUrl => manifestUrl === event.request.url);

    const cache = await caches.open(cacheName);

    if (shouldServeIndexHtml) {
        const indexRequest = new URL('index.html', baseUrl).href;
        const cachedIndex = await cache.match(indexRequest);
        if (cachedIndex) return cachedIndex;
    }

    const cachedResponse = await cache.match(event.request);
    return cachedResponse || fetch(event.request);
}
