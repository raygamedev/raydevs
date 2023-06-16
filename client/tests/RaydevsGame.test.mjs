
import puppeteer from "puppeteer";
import assert from "assert";
const PUPPETEER_CONFIG = {
    url: 'http://127.0.0.1:3000',
    viewPort: { width: 1920, height: 1080 },
    networkState: { until: 'networkidle1' },
    puppeteerLaunchOption: {
      headless: "new",
      hasTouch: true,
      args: ['--use-gl=desktop', '--no-sandbox', '--disable-setuid-sandbox', '--disable-infobars', '--proxy-server=http://localhost:6000']
    }
};

const initPuppeteer = async () => {
const browser = await puppeteer.launch(PUPPETEER_CONFIG.puppeteerLaunchOption);
const page = await browser.newPage();
await page.setViewport(PUPPETEER_CONFIG.viewPort);
// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-ignore
await page.goto(PUPPETEER_CONFIG.url, PUPPETEER_CONFIG.networkState);
return { browser, page };
};

const getIsGameLoaded = async (page) => await page.evaluate(() =>
  document.querySelector("[data-testid='raydevs-game-button']").getAttribute('data-is-game-loaded') === 'true');

const getIsGamePlaying = async (page) => await page.evaluate(() =>
    document.querySelector("[data-testid='raydevs-game-button']").getAttribute('data-is-game-playing') === 'true');

describe('Raydevs Game Loads', function () {
    this.timeout(30000)
    let page;
    let browser;
        it ('Validate Game Button is loading', async () => {
            console.log("Something")
    });
    before(async function () {
        const p = await initPuppeteer();
        page = p.page;
        browser = p.browser;
    });
    it ('Validate Game Button is loading', async () => {
        await page.waitForSelector("[data-testid='raydevs-game-button']", { timeout: 10000 });
        const isGameLoaded = await getIsGameLoaded(page);
        assert(!isGameLoaded, 'Game Button in loading state')
    });
    it('Wait for Raydevs Game to load', async () => {
        let isGameLoaded = false;
        const attempts = 10;
        const interval = 1000;
        for(let i = 0; i < attempts; i++) {
            isGameLoaded = await getIsGameLoaded(page);
            if(isGameLoaded) break;
            await page.waitForTimeout(interval);
        }
        assert(isGameLoaded, 'Game did not load');
    });
    it('Play Raydevs Game', async () => {
        await page.click("[data-testid='raydevs-game-button']");
        assert(await getIsGamePlaying(page), 'Game did not start');
    })

    after(async function () {
        await browser.close();
    })
});