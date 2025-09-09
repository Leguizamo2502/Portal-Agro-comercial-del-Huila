// src/test.ts
import 'zone.js/testing';
import { getTestBed, TestBed } from '@angular/core/testing';
import {
  BrowserDynamicTestingModule,
  platformBrowserDynamicTesting,
} from '@angular/platform-browser-dynamic/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
// Si usas interceptores DI y necesitas que participen en los tests, descomenta:
// import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

// Inicializa el entorno de pruebas Angular
getTestBed().initTestEnvironment(
  BrowserDynamicTestingModule,
  platformBrowserDynamicTesting(),
);

// Providers globales para TODOS los tests
beforeEach(() => {
  TestBed.configureTestingModule({
    providers: [
      // Si no usas interceptores: solo esta línea
      provideHttpClientTesting(),

      // Si SÍ usas interceptores por DI en los tests:
      // provideHttpClient(withInterceptorsFromDi()),
      // provideHttpClientTesting(),
    ],
  });
});
