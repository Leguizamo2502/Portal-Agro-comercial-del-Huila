export interface ProductSelectModel {
    id:           number;
    name:         string;
    description:  string;
    price:        number;
    unit:         string;
    production:   string;
    stock:        number;
    status:       boolean;
    categoryId:   number;
    categoryName: string;
    images:       ImageProduct[];
    personName:   string;
    farmId:       number;
    farmName:     string;
}

export interface ImageProduct {
    id:       number;
    imageUrl: string;
}
