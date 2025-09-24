import { Pipe, PipeTransform } from '@angular/core';
import { SocialNetwork } from '../../models/producer/producer.model';

@Pipe({
  name: 'socialNetworkIcon'
})
export class SocialNetworkIconPipe implements PipeTransform {
  transform(value: SocialNetwork): string {
    switch (value) {
      case SocialNetwork.Website:   return 'public';
      case SocialNetwork.Facebook:  return 'facebook';
      case SocialNetwork.Instagram: return 'photo_camera';
      case SocialNetwork.Whatsapp:  return 'chat';
      case SocialNetwork.X:         return 'alternate_email';
      default:                      return 'share';
    }
  }
}