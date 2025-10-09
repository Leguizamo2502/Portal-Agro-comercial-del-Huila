import { Injectable } from '@angular/core';
import { driver, DriveStep } from 'driver.js';
import 'driver.js/dist/driver.css';

@Injectable({
  providedIn: 'root'
})
export class DriverJsService {

  startTour(steps: DriveStep[]) {
    const driverObj = driver({
      showProgress: true,
      animate: true,
      overlayOpacity: 0.6,
      allowClose: true,
      steps,

      nextBtnText: 'Siguiente',
      prevBtnText: 'Anterior',
      doneBtnText: 'Finalizar'
    });

    driverObj.drive();
  }
}
