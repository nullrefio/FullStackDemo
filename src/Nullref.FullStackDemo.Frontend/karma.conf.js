// Karma configuration file, see link for more information
// https://karma-runner.github.io/1.0/config/configuration-file.html

// if we skipped downloading Chromium for Puppeteer, do not overwrite the executable path
// this supports local users outside of the docker container
if (process.env.PUPPETEER_SKIP_CHROMIUM_DOWNLOAD !== 'true') {
  process.env.CHROMIUM_BIN = require('puppeteer').executablePath();
}

module.exports = function (config) {
  config.set({
    basePath: '',
    frameworks: ['jasmine', '@angular-devkit/build-angular'],
    plugins: [
      require('karma-jasmine'),
      require('karma-chrome-launcher'),
      require('karma-jasmine-html-reporter'),
      require('karma-coverage-istanbul-reporter'),
      require('@angular-devkit/build-angular/plugins/karma')
    ],
    client: {
      clearContext: false // leave Jasmine Spec Runner output visible in browser
    },
    coverageIstanbulReporter: {
      dir: require('path').join(__dirname, 'coverage'),
      reports: ['cobertura'],
      fixWebpackSourcePaths: true
    },
    reporters: ['coverage-istanbul'],
    port: 9876,
    colors: true,
    logLevel: config.LOG_INFO,
    autoWatch: true,
    browsers: ['ChromiumHeadlessNoSandbox'],
    customLaunchers: {
      ChromiumHeadlessNoSandbox: {
        base: 'ChromiumHeadless',
        flags: [
          '--no-sandbox'
        ]
      }
    },
    singleRun: false,
    restartOnFileChange: true,
    browserNoActivityTimeout: 400000,
  });
};
