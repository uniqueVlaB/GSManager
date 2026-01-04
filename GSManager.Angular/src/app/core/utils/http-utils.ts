import { HttpParams } from '@angular/common/http';

export class HttpUtils {
  /**
   * Converts a plain object into HttpParams, filtering out null, undefined, and empty string values.
   * Handles arrays by appending multiple values with the same key.
   */
  static createParams(params: Record<string, any> | undefined | null): HttpParams {
    let httpParams = new HttpParams();

    if (!params) {
      return httpParams;
    }

    Object.entries(params).forEach(([key, value]) => {
      if (value === undefined || value === null) {
        return;
      }

      if (Array.isArray(value)) {
        if (value.length > 0) {
          value.forEach(v => {
            if (v !== undefined && v !== null) {
              httpParams = httpParams.append(key, v);
            }
          });
        }
      } else if (value !== '') {
        // Convert non-string primitives to string
        httpParams = httpParams.set(key, String(value));
      }
    });

    return httpParams;
  }
}
