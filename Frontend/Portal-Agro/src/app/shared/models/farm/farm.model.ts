export interface FarmSelectModel {
    id:             number;
    name:           string;
    latitude:       string;
    longitude:      string;
    cityName:       string;
    departmentName: string;
    producerName:   string;
    images:         Image[];
}

export interface Image {
    id:       number;
    imageUrl: string;
}
