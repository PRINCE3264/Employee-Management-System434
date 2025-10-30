
// import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
// import { provideRouter } from '@angular/router';
// import { provideHttpClient, withInterceptors } from '@angular/common/http';
// import { routes } from './app.routes';
// import { provideNativeDateAdapter } from '@angular/material/core';
// import { TokenHttpInterceptor } from './services/token-http-interceptor';
// export const appConfig: ApplicationConfig = {
//   providers: [
//     provideZoneChangeDetection({ eventCoalescing: true }),
//     provideRouter(routes),
//     provideHttpClient(withInterceptors([TokenHttpInterceptor])),
//     provideNativeDateAdapter(),
//   ]
// };

// import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
// import { provideRouter } from '@angular/router';
// import { provideHttpClient, withInterceptors } from '@angular/common/http';
// import { routes } from './app.routes';
// import { provideNativeDateAdapter } from '@angular/material/core';
// import { tokenHttpInterceptor } from './services/token-http-interceptor';

// export const appConfig: ApplicationConfig = {
//   providers: [
//     provideZoneChangeDetection({ eventCoalescing: true }),
//     provideRouter(routes),

//     // Use the functional interceptor here:
//     provideHttpClient(withInterceptors([tokenHttpInterceptor])),

//     provideNativeDateAdapter(),
//   ]
// };


import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { routes } from './app.routes';
import { provideNativeDateAdapter } from '@angular/material/core';
import { tokenHttpInterceptor } from './services/token-http-interceptor'; // ✅ Correct functional interceptor

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptors([tokenHttpInterceptor])), // ✅ Correct usage
    provideNativeDateAdapter(),
  ]
};
