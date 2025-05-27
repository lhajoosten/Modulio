import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthorizedGuard implements CanActivate {

  constructor(private router: Router) { }

  canActivate(): Observable<boolean> | Promise<boolean> | boolean {
    // Check if user is authenticated (implement your auth logic here)
    const isAuthenticated = this.checkAuthentication();

    if (!isAuthenticated) {
      this.router.navigate(['/login']);
      return false;
    }

    return true;
  }

  private checkAuthentication(): boolean {
    // Implement your authentication check logic
    // e.g., check token in localStorage, session, etc.
    return !!localStorage.getItem('authToken');
  }
}
