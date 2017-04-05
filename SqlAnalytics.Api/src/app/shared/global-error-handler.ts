import { ErrorHandler,Inject } from '@angular/core';
import { ErrorNotifyService } from './error-notify-service'

export default class GlobalErrorHandler implements ErrorHandler {
  constructor(@Inject(ErrorNotifyService)private errorNotifyService: ErrorNotifyService) {

  }
  handleError(error) {
    let unwrappedError = error.originalError || error;
    setTimeout(() => {
      this.errorNotifyService.error(unwrappedError);
    }, 1);
  }

}