// shared/menu/menu.models.ts
export interface MenuNode {
  id: string;
  label: string;
  icon?: string;
  route?: string[];        // ['/account', ...segments]
  children?: MenuNode[];
}
