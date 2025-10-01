import { Pipe, PipeTransform } from '@angular/core';
import { SocialNetwork } from '../../models/producer/producer.model';

@Pipe({
  name: 'socialNetworkLabel'
})
export class SocialNetworkLabelPipe implements PipeTransform {
  transform(value: SocialNetwork): string {
    switch (value) {
      case SocialNetwork.Website:   return 'Sitio web';
      case SocialNetwork.Facebook:  return 'Facebook';
      case SocialNetwork.Instagram: return 'Instagram';
      case SocialNetwork.Whatsapp:  return 'WhatsApp';
      case SocialNetwork.X:         return 'X';
      default:                      return 'Red';
    }
  }
}