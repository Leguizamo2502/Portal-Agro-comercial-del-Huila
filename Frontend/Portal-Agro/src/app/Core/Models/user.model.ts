export interface UserSelectModel{
    firstName:       string;
    lastName:       string;
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


export interface PersonUpdateModel{
    firstName:       string;
    lastName:       string;

    // identification: string;
    address:        string;
    phoneNumber:    string;
    // email:          string;
}