import { ValidationErrors, Validators } from '@angular/forms';

export abstract class Util {
  static validatedRecordsPerPage(pageSizeParam: any): Readonly<number> {
    let recordsPerPage = 10;
    if (pageSizeParam === null || pageSizeParam === undefined) {
      return recordsPerPage;
    }

    const parsedPageSizeParam: number = parseInt(pageSizeParam as unknown as string, 10) || 10;
    if ([5, 10, 20].includes(parsedPageSizeParam)) {
      recordsPerPage = parsedPageSizeParam;
    }

    return recordsPerPage;
  }

  static validatedPageNumber(pageParam: any): Readonly<number> {
    let page = 1;
    if (pageParam === null || pageParam === undefined) {
      return page;
    }
    const parsedPageParam: number = parseInt(pageParam as unknown as string, 10) || 0;
    if (parsedPageParam > 0) {
      page = parsedPageParam;
    }
    return page;
  }

  static formatFileSize(size: number): string {
    const kb = 1024;
    const mb = kb * 1024;
    if (size < kb) {
      return size.toString() + ' bytes';
    } else if (size < mb) {
      return Math.round(size / kb).toString() + ' KB';
    } else {
      return Math.round(size / mb).toString() + 'MB';
    }
  }
}

// eslint-disable-next-line @typescript-eslint/member-delimiter-style
export const metaDefault = (meta: { identifier: any, maxLength: any, default: any, readOnly: any, required: any, nullable: any }, key: string): any => {
  // The input should be a generated Metadata object
  const keys = Object.keys(meta);
  if (keys.indexOf('required') === -1 || keys.indexOf('maxLength') === -1) {
    throw new Error('Unknown metadata');
  }
  const v = meta.default[key];
  if (v !== undefined) {
    return v;
  }
  return null;
};

// eslint-disable-next-line @typescript-eslint/member-delimiter-style
export const metaValidators = (meta: { identifier: any, maxLength: any, minLength: any, default: any, readOnly: any, required: any, nullable: any }, key: string): ValidationErrors[] => {
  // The input should be a generated Metadata object
  const keys = Object.keys(meta);
  if (keys.indexOf('required') === -1 || keys.indexOf('maxLength') === -1) {
    throw new Error('Unknown metadata');
  }
  const required = meta.required[key] === true;
  const maxLength: number = meta.maxLength[key];
  const minLength: number = meta.minLength[key];
  const retval: ValidationErrors[] = [];
  if (required) {
    retval.push(Validators.required);
  }
  if (maxLength !== undefined) {
    retval.push(Validators.maxLength(maxLength));
  }
  if (minLength !== undefined) {
    retval.push(Validators.minLength(minLength));
  }
  return retval;
};

// Setup a full form field based on a metadata object
// eslint-disable-next-line @typescript-eslint/member-delimiter-style
export const metaFormField = (meta: { identifier: any, maxLength: any, minLength: any, default: any, readOnly: any, required: any, nullable: any }, key: string): any[] =>
  [metaDefault(meta, key), metaValidators(meta, key)];
