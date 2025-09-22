export interface ProducerSelectModel {
  code: string;
  fullName: string;
  phoneNumber: string;
  email: string;
  qrUrl: string;
  description: string;
  networks:ProducerSocialSelectModel[]
  id: number;
}

export enum SocialNetwork {
  Website = 0,
  Facebook = 1,
  Instagram = 2,
  Whatsapp = 3,
  X = 4
}

export interface ProducerSocialCreateModel {
  network: SocialNetwork;   // numérico
  url: string;
}

export interface ProducerSocialSelectModel {
  id: number;
  network: SocialNetwork; 
  url: string;
}

