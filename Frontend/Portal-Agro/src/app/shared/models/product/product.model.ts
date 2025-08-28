export interface ApiOk {
  isSuccess: boolean;   // si tu backend devuelve PascalCase, ver nota abajo
  message: string;
  id?: number;
}

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

  // Legacy (compat)
  farmId?: number;
  farmName: string;
  cityName: string;
  departmentName: string;

  // NUEVO: todas las fincas asociadas
  farmIds?: number[];

  isFavorite: boolean;

  // mockups opcionales
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
  farmIds: number[];               // << antes: farmId
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
  images?: File[];
  farmIds: number[];               // << antes: farmId
  imagesToDelete?: string[];
}

export interface FavoriteCreateRequest {
  productId: number;
}

export interface ReviewModel {
  user: string;
  avatar: string;
  date: Date;
  comment: string;
  rating: number;
}
