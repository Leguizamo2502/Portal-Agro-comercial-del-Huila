import { HttpContextToken } from '@angular/common/http';

export const OPTIONAL_AUTH = new HttpContextToken<boolean>(() => false);
