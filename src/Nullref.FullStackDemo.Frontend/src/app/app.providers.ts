import { APP_INITIALIZER, FactoryProvider } from '@angular/core';
import { ApiConfiguration } from './api/api-configuration';

/**
 * A Provider that will read the configuration file during app initialization.
 */

export const runtimeConfigAppInitializer: FactoryProvider = {
  provide: APP_INITIALIZER,
  multi: true,
  deps: [ApiConfiguration],
  useFactory: (apiConfig: ApiConfiguration) =>
    () => {
      // After fetching the runtime config, set the root URL for the API services.
      apiConfig.rootUrl = 'http://localhost:50001';
    }
};

export const APP_MODULE_PROVIDERS = [
  runtimeConfigAppInitializer,
];
