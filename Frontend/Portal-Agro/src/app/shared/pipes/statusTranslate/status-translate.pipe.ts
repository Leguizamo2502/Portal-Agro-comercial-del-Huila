import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'statusTranslate'
})
export class StatusTranslatePipe implements PipeTransform {

   private readonly statusMap: Record<string, string> = {
    PendingReview: 'Pendiente de revisión',
    AcceptedAwaitingUser: 'Aceptado (esperando usuario)',
    Completed: 'Completado',
    Rejected: 'Rechazado',
    Disputed: 'En disputa'
  };

  transform(value: string): string {
    return this.statusMap[value] || value;
  }

}
