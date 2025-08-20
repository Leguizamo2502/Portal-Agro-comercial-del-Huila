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

  // 👇 Opcionales (solo mockup en frontend)
  orders?: number;
  categories?: string[];
  moreInfo?: string;
  location?: string;
  reviews?: ReviewModel[];
}

export interface ProductImageSelectModel {
  id: number;
  fileName: string;
  imageUrl: string;
  publicId: string;
  productId: number;
}

export interface ReviewModel {
  user: string;
  avatar: string;
  date: Date;
  comment: string;
}
