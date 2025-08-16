export interface Review {
    user: string;
    avatar: string;
    rating: number;
    comment: string;
    date: string;
}

export interface Product {
    id: number;
    name: string;
    image: string;
    orders: number;
    producer: string;
    categories: string[];
    stock: number;
    price: number;
    description: string;
    moreInfo: string;
    location: string;
    reviews: Review[];
    thumbnails?: string[];
}