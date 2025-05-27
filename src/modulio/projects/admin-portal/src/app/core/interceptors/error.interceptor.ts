export const ErrorInterceptor = () => {
  return {
    provide: 'HTTP_INTERCEPTORS',
    useClass: ErrorInterceptor,
    multi: true
  };
}
