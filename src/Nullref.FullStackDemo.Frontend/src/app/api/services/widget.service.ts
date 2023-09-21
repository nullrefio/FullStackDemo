/* tslint:disable */
/* eslint-disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpContext } from '@angular/common/http';
import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';
import { RequestBuilder } from '../request-builder';
import { Observable } from 'rxjs';
import { map, filter } from 'rxjs/operators';

import { IdResponseModel } from '../models/id-response-model';
import { WidgetModel } from '../models/widget-model';
import { WidgetModelPaginatedResponseModel } from '../models/widget-model-paginated-response-model';
import { WidgetUpdateModel } from '../models/widget-update-model';


/**
 * Widget API
 */
@Injectable({
  providedIn: 'root',
})
export class WidgetService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation apiV1WidgetsIdGet
   */
  static readonly ApiV1WidgetsIdGetPath = '/api/v1/widgets/{id}';

  /**
   * Get a single widget.
   *
   *
   *
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiV1WidgetsIdGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiV1WidgetsIdGet$Plain$Response(params: {
    id: string;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<WidgetModel>> {

    const rb = new RequestBuilder(this.rootUrl, WidgetService.ApiV1WidgetsIdGetPath, 'get');
    if (params) {
      rb.path('id', params.id, {"style":"simple"});
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<WidgetModel>;
      })
    );
  }

  /**
   * Get a single widget.
   *
   *
   *
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiV1WidgetsIdGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiV1WidgetsIdGet$Plain(params: {
    id: string;
  },
  context?: HttpContext

): Observable<WidgetModel> {

    return this.apiV1WidgetsIdGet$Plain$Response(params,context).pipe(
      map((r: StrictHttpResponse<WidgetModel>) => r.body as WidgetModel)
    );
  }

  /**
   * Get a single widget.
   *
   *
   *
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiV1WidgetsIdGet()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiV1WidgetsIdGet$Response(params: {
    id: string;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<WidgetModel>> {

    const rb = new RequestBuilder(this.rootUrl, WidgetService.ApiV1WidgetsIdGetPath, 'get');
    if (params) {
      rb.path('id', params.id, {"style":"simple"});
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<WidgetModel>;
      })
    );
  }

  /**
   * Get a single widget.
   *
   *
   *
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiV1WidgetsIdGet$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiV1WidgetsIdGet(params: {
    id: string;
  },
  context?: HttpContext

): Observable<WidgetModel> {

    return this.apiV1WidgetsIdGet$Response(params,context).pipe(
      map((r: StrictHttpResponse<WidgetModel>) => r.body as WidgetModel)
    );
  }

  /**
   * Path part for operation apiV1WidgetsIdPut
   */
  static readonly ApiV1WidgetsIdPutPath = '/api/v1/widgets/{id}';

  /**
   * Update a widget.
   *
   *
   *
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiV1WidgetsIdPut()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiV1WidgetsIdPut$Response(params: {
    id: string;
    body: WidgetUpdateModel
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, WidgetService.ApiV1WidgetsIdPutPath, 'put');
    if (params) {
      rb.path('id', params.id, {"style":"simple"});
      rb.body(params.body, 'application/*+json');
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: '*/*',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return (r as HttpResponse<any>).clone({ body: undefined }) as StrictHttpResponse<void>;
      })
    );
  }

  /**
   * Update a widget.
   *
   *
   *
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiV1WidgetsIdPut$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiV1WidgetsIdPut(params: {
    id: string;
    body: WidgetUpdateModel
  },
  context?: HttpContext

): Observable<void> {

    return this.apiV1WidgetsIdPut$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiV1WidgetsIdDelete
   */
  static readonly ApiV1WidgetsIdDeletePath = '/api/v1/widgets/{id}';

  /**
   * Delete a widget.
   *
   *
   *
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiV1WidgetsIdDelete()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiV1WidgetsIdDelete$Response(params: {
    id: string;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, WidgetService.ApiV1WidgetsIdDeletePath, 'delete');
    if (params) {
      rb.path('id', params.id, {"style":"simple"});
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: '*/*',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return (r as HttpResponse<any>).clone({ body: undefined }) as StrictHttpResponse<void>;
      })
    );
  }

  /**
   * Delete a widget.
   *
   *
   *
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiV1WidgetsIdDelete$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiV1WidgetsIdDelete(params: {
    id: string;
  },
  context?: HttpContext

): Observable<void> {

    return this.apiV1WidgetsIdDelete$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiV1WidgetsGet
   */
  static readonly ApiV1WidgetsGetPath = '/api/v1/widgets';

  /**
   * Get a paginated list of widgets.
   *
   *
   *
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiV1WidgetsGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiV1WidgetsGet$Plain$Response(params: {
    PageNumber: number;
    PageSize: number;
    Order?: string;
    Search?: string;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<WidgetModelPaginatedResponseModel>> {

    const rb = new RequestBuilder(this.rootUrl, WidgetService.ApiV1WidgetsGetPath, 'get');
    if (params) {
      rb.query('PageNumber', params.PageNumber, {"style":"form"});
      rb.query('PageSize', params.PageSize, {"style":"form"});
      rb.query('Order', params.Order, {"style":"form"});
      rb.query('Search', params.Search, {"style":"form"});
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<WidgetModelPaginatedResponseModel>;
      })
    );
  }

  /**
   * Get a paginated list of widgets.
   *
   *
   *
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiV1WidgetsGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiV1WidgetsGet$Plain(params: {
    PageNumber: number;
    PageSize: number;
    Order?: string;
    Search?: string;
  },
  context?: HttpContext

): Observable<WidgetModelPaginatedResponseModel> {

    return this.apiV1WidgetsGet$Plain$Response(params,context).pipe(
      map((r: StrictHttpResponse<WidgetModelPaginatedResponseModel>) => r.body as WidgetModelPaginatedResponseModel)
    );
  }

  /**
   * Get a paginated list of widgets.
   *
   *
   *
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiV1WidgetsGet()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiV1WidgetsGet$Response(params: {
    PageNumber: number;
    PageSize: number;
    Order?: string;
    Search?: string;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<WidgetModelPaginatedResponseModel>> {

    const rb = new RequestBuilder(this.rootUrl, WidgetService.ApiV1WidgetsGetPath, 'get');
    if (params) {
      rb.query('PageNumber', params.PageNumber, {"style":"form"});
      rb.query('PageSize', params.PageSize, {"style":"form"});
      rb.query('Order', params.Order, {"style":"form"});
      rb.query('Search', params.Search, {"style":"form"});
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<WidgetModelPaginatedResponseModel>;
      })
    );
  }

  /**
   * Get a paginated list of widgets.
   *
   *
   *
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiV1WidgetsGet$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiV1WidgetsGet(params: {
    PageNumber: number;
    PageSize: number;
    Order?: string;
    Search?: string;
  },
  context?: HttpContext

): Observable<WidgetModelPaginatedResponseModel> {

    return this.apiV1WidgetsGet$Response(params,context).pipe(
      map((r: StrictHttpResponse<WidgetModelPaginatedResponseModel>) => r.body as WidgetModelPaginatedResponseModel)
    );
  }

  /**
   * Path part for operation apiV1WidgetsPost
   */
  static readonly ApiV1WidgetsPostPath = '/api/v1/widgets';

  /**
   * Create a widget.
   *
   *
   *
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiV1WidgetsPost$Plain()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiV1WidgetsPost$Plain$Response(params: {
    body: WidgetUpdateModel
  },
  context?: HttpContext

): Observable<StrictHttpResponse<IdResponseModel>> {

    const rb = new RequestBuilder(this.rootUrl, WidgetService.ApiV1WidgetsPostPath, 'post');
    if (params) {
      rb.body(params.body, 'application/*+json');
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<IdResponseModel>;
      })
    );
  }

  /**
   * Create a widget.
   *
   *
   *
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiV1WidgetsPost$Plain$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiV1WidgetsPost$Plain(params: {
    body: WidgetUpdateModel
  },
  context?: HttpContext

): Observable<IdResponseModel> {

    return this.apiV1WidgetsPost$Plain$Response(params,context).pipe(
      map((r: StrictHttpResponse<IdResponseModel>) => r.body as IdResponseModel)
    );
  }

  /**
   * Create a widget.
   *
   *
   *
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiV1WidgetsPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiV1WidgetsPost$Response(params: {
    body: WidgetUpdateModel
  },
  context?: HttpContext

): Observable<StrictHttpResponse<IdResponseModel>> {

    const rb = new RequestBuilder(this.rootUrl, WidgetService.ApiV1WidgetsPostPath, 'post');
    if (params) {
      rb.body(params.body, 'application/*+json');
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<IdResponseModel>;
      })
    );
  }

  /**
   * Create a widget.
   *
   *
   *
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiV1WidgetsPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiV1WidgetsPost(params: {
    body: WidgetUpdateModel
  },
  context?: HttpContext

): Observable<IdResponseModel> {

    return this.apiV1WidgetsPost$Response(params,context).pipe(
      map((r: StrictHttpResponse<IdResponseModel>) => r.body as IdResponseModel)
    );
  }

}
