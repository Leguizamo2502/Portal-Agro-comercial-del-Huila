export interface UserSelectModel{
    fullName:       string;
    identification: string;
    address:        string;
    phoneNumber:    string;
    email:          string;
    cityId:         number;
    cityName:       string;
    active:         boolean;
    roles:          string[];
    id:             number;
}