import {
  ApplicationConfig,
  LOCALE_ID,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { credentialsInterceptor } from './Core/interceptors/credentials.interceptor';
import { authInterceptor } from './Core/interceptors/auth/auth.interceptor';
import { provideCharts, withDefaultRegisterables } from 'ng2-charts';


import { registerLocaleData } from '@angular/common';
import localeEsCO from '@angular/common/locales/es-CO';
registerLocaleData(localeEsCO);

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes, withComponentInputBinding()),
    provideHttpClient(
      withInterceptors([credentialsInterceptor, authInterceptor])
    ),
    { provide: LOCALE_ID, useValue: 'es-CO' }, provideCharts(withDefaultRegisterables())
  ],
};
