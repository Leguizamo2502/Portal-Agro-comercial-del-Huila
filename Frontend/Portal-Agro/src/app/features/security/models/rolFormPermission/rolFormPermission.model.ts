export interface RolFormPermissionSelectModel{
    id : number;
    rolId:number;
    rolName: string;
    formId:number
    formName:string
    permissionId: number;
    permissionName:string;
}

export interface RolFormPermissionRegisterModel{
    // id : number;
    rolId:number;
    formId: number;
    permissionId: number;

}