export interface FarmSelectModel {
  id: number;
  name: string;
  latitude: string;
  longitude: string;
  cityName: string;
  departmentName: string;
  producerName: string;
  images: FarmImageSelectModel[];
}

export interface FarmImageSelectModel {
  id: number;
  fileName: string;
  imageUrl: string;
  publicId: string;
  farmId: number;
}

export interface FarmWithProducerRegisterModel {
  description: string;
  name: string;
  hectares: number;
  altitude: number;
  latitude: number;
  longitude: number;

  images: File[];
  cityId: number;
}

export interface FarmRegisterModel {
  name: string;
  hectares: number;
  altitude: number;
  latitude: number;
  longitude: number;

  images: File[];
  cityId: number;
}

export interface FarmUpdateModel {
  id: number;
  name: string;
  hectares: number;
  altitude: number;
  latitude: number;
  longitude: number;
  images?: File[];
  imagesToDelete?: string[]; // usa esto solo si tu API soporta borrar por PublicId
  cityId: number;
}
