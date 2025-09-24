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
  shippingIncluded : boolean;
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

  producerCode: string;  
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
  shippingIncluded : boolean;
  status: boolean;
  categoryId: number;
  images?: File[];
  farmIds: number[];           
}

export interface ProductUpdateModel {
  id: number;
  name: string;
  description: string;
  price: number;
  unit: string;
  production: string;
  stock: number;
  shippingIncluded : boolean;
  status: boolean;
  categoryId: number;
  images?: File[];
  farmIds: number[];               
  imagesToDelete?: string[];
}

export interface FavoriteCreateRequest {
  productId: number;
}

export interface ReviewRegisterModel {
  productId: number;
  rating: number;         
  comment: string;
}

export interface ReviewSelectModel {
  id: number;              
  productId: number;
  userId: number;
  userName: string;
  rating: number;         
  comment: string;
  createAt: string;       
}
