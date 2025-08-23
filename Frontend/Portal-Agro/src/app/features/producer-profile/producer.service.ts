import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Producer } from '../../shared/models/producer-profile/producer-profile.model';

@Injectable({ providedIn: 'root' })
export class ProducerService {

    getProducerProfile(): Observable<Producer> {
        return of({
            id: 1,
            description: "Hola soy Juan la persona encargada del cultivo...",
            code: "PROD123",
            qrUrl: "https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=PROD123",
            user: {
                id: 1,
                name: "Juan Felipe",
                email: "veresauriot@gmail.com",
                active: true,
                imageUrl: "https://picsum.photos/200/200?random=1"
            },
            farms: [
            {
                id: 1,
                name: "Villa Leo",
                location: "Nariño",
                hectares: 10,
                altitude: 2000,
                latitude: 1.2,
                longitude: -77.3,
                imageUrl: "https://picsum.photos/400/200?random=10"
            },
            {
                id: 2,
                name: "Farm Orgánica",
                location: "Antioquia",
                hectares: 15,
                altitude: 1500,
                latitude: 2.1,
                longitude: -76.1,
                imageUrl: "https://picsum.photos/400/200?random=11"
            }],
            products: [
            {
                id: 1,
                name: "Plátano Dominico Hartón",
                price: 2000,
                description: "Fruto amarillo tropical",
                stock: 100,
                imageUrl: "https://picsum.photos/200/150?random=20"
            },
            {
                id: 2,
                name: "Naranja Dulce",
                price: 1500,
                description: "Cítrico jugoso",
                stock: 50,
                imageUrl: "https://picsum.photos/200/150?random=21"
            },
            {
                id: 3,
                name: "Papa Amarilla",
                price: 1800,
                description: "Tubérculo andino",
                stock: 80,
                imageUrl: "https://picsum.photos/200/150?random=22"
            }]
        });
    }
}
