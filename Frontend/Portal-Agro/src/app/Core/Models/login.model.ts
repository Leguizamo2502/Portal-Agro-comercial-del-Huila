export interface LoginModel {
    email: string;
    password: string;
}

export interface LoginResponseModel {
    id:    string;
    email: string;
    roles: string[];
}

// core/auth/auth.models.ts
export interface UserMeDto {
  id: number;
  fullName: string;
  email: string;
  roles: string[];
  permissions: string[];        // ["Crear", "Leer", ...]  ideal: códigos estables
  menu: MenuModuleDto[];
}
export interface MenuModuleDto {
  id: number;
  name: string;
  description?: string | null;
  forms: FormMeDto[];
}
export interface FormMeDto {
  id: number;
  name: string;
  description?: string | null;
  url: string | null;         // asegúrate de rellenarlo desde el back
  permissions: string[];        // por form
}