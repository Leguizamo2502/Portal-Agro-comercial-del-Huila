export interface UserSelectModel{
    firstName:       string;
    lastName:       string;
    identification: string;
    address:        string;
    phoneNumber:    string;
    email:          string;
    cityId:         number;
    departmentId: number;
    cityName:       string;
    active:         boolean;
    roles:          string[];
    id:             number;
}


export interface PersonUpdateModel{
    firstName:       string;
    lastName:       string;
    cityId: number;

    // identification: string;
    address:        string;
    phoneNumber:    string;
    // email:          string;
}