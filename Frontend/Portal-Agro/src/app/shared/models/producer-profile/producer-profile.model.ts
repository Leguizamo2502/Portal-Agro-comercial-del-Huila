export interface Producer {
    id: number;
    description: string;
    code: string;
    qrUrl: string;
    user: {
    id: number;
    name: string;
    email: string;
    active: boolean;
    imageUrl: string;
    };

    farms: {
    id: number;
    name: string;
    location: string;
    hectares: number;
    altitude: number;
    latitude: number;
    longitude: number;
    imageUrl: string;
    }[];
    
    products: {
    id: number;
    name: string;
    price: number;
    description: string;
    stock: number;
    imageUrl: string;
    }[];
}
