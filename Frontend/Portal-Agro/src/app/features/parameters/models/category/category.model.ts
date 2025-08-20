export interface CategorySelectModel {
  id: number;
  name: string;
  parentCategoryId: number | null;
  parentName?: string;
}

export interface CategoryRegistertModel {
  id: number;
  name: string;
  parentCategoryId: number | null;
}
