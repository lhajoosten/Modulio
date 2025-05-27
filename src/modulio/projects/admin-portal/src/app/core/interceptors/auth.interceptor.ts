export const AuthInterceptor = () => {
  return {
    provide: 'HTTP_INTERCEPTORS',
    useClass: AuthInterceptor,
    multi: true
  };
}
