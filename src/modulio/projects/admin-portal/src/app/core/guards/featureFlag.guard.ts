import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class FeatureFlagGuard implements CanActivate {

  constructor(
    private router: Router,
    // Inject your feature flag service here
    // private featureFlagService: FeatureFlagService
  ) { }

  canActivate(route: ActivatedRouteSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    const featureFlag = route.data['featureFlag'] as string;

    if (!featureFlag) {
      return true;
    }

    // Replace this with your actual feature flag service call
    // return this.featureFlagService.isEnabled(featureFlag).pipe(
    //   map(isEnabled => {
    //     if (!isEnabled) {
    //       this.router.navigate(['/not-found']);
    //       return false;
    //     }
    //     return true;
    //   })
    // );

    // Temporary implementation - replace with actual service
    console.warn('FeatureFlagGuard: Feature flag service not implemented');
    return true;
  }
}
