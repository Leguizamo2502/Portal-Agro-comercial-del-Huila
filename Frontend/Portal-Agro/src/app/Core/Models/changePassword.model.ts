export interface ChangePasswordModel {
    currentPassword: string;
    newPassword: string;
    confirmNewPassword?: string;
}

export interface RecoverPasswordModel{
    email: string;
}
export interface RecoverPasswordConfirmModel{
    email: string;
    code : string;
    newPassword: string;
}