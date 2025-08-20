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


