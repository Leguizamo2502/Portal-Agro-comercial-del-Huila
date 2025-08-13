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
    images:       ProductImageSelectModel[];
    personName:   string;
    farmId:       number;
    farmName:     string;
}

export interface ProductImageSelectModel {
  id: number;
  fileName: string;
  imageUrl: string;
  publicId: string;
  productId: number;
}
