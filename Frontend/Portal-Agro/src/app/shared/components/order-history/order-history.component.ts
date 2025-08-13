import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

interface Order {
  id: string;
  Name: string;
  Date: string;
  State: 'Aceptado' | 'En espera';
  valor: number;
}

@Component({
  selector: 'app-order-history',
  imports: [CommonModule],
  templateUrl: './order-history.component.html',
  styleUrl: './order-history.component.css'
})
export class OrderHistoryComponent {
  
  stats = {
    total: 5,
    completadas: 3,
    pendientes: 2
  };

  orders: Order[] = [
    { id: '#1', Name: 'Constumers1', Date: '2023-08-15', State: 'Aceptado', valor: 100 },
    { id: '#2', Name: 'Constumers2', Date: '2023-08-10', State: 'Aceptado', valor: 100 },
    { id: '#3', Name: 'Constumers3', Date: '2023-08-05', State: 'Aceptado', valor: 100 },
    { id: '#4', Name: 'Constumers4', Date: '2023-07-20', State: 'En espera', valor: 100 },
    { id: '#5', Name: 'Constumers5', Date: '2023-07-15', State: 'En espera', valor: 100 }
  ];

  constructor() { }

  ngOnInit(): void { }
}
