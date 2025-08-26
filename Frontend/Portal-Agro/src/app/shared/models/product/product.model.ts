export interface ProductSelectModel {
  id: number;
  name: string;
  description: string;
  price: number;
  unit: string;
  production: string;
  stock: number;
  status: boolean;
  categoryId: number;
  categoryName: string;
  images: ProductImageSelectModel[];
  personName: string;
  farmId: number;
  farmName: string;
  cityName: string;
  departmentName: string;
  isFavorite: boolean;
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

export interface ProductRegisterModel {
  name: string;
  description: string;
  price: number;
  unit: string;
  production: string;
  stock: number;
  status: boolean;
  categoryId: number;
  images?: File[];
  farmId: number;
}

export interface ProductUpdateModel {
  id: number;
  name: string;
  description: string;
  price: number;
  unit: string;
  production: string;
  stock: number;
  status: boolean;
  categoryId: number;
  images?: File[]; // Archivos nuevos a subir
  farmId: number;
  imagesToDelete?: string[]; // PublicId o nombres de archivos a eliminar
}

export interface FavoriteCreateRequest{
  productId:number
}

export interface ReviewModel {
  user: string;
  avatar: string;
  date: Date;
  comment: string;

  // prueba de calificacion
  rating: number;
}
