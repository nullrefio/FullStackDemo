/* tslint:disable */
/* eslint-disable */
import { FruitConstants } from './fruit-constants';
export interface WidgetUpdateModel {

  /**
   * The widget code
   */
  code: string;

  /**
   * A tooltip for description
   */
  description: string;
  isActive: boolean;
  myFruit: FruitConstants;

  /**
   * What state is this thing from?
   */
  state: string;
}

export const WidgetUpdateModelMetadata = {
  identifier: {
    code: 'code',
    description: 'description',
    isActive: 'isActive',
    myFruit: 'myFruit',
    state: 'state',
  },
  maxLength: {
    code: 100,
    description: 500,
    state: 50,
  },
  minLength: {
    code: 1,
    description: 1,
    state: 1,
  },
  readOnly: {
    code: false,
    description: false,
    isActive: false,
    myFruit: false,
    state: false,
  },
  required: {
    code: true,
    description: true,
    isActive: true,
    myFruit: true,
    state: true,
  },
  nullable: {
    code: false,
    description: false,
    isActive: false,
    myFruit: false,
    state: false,
  },
  default: {
    code: 'My defined default value',
    description: 'what....??',
    isActive: true,
    myFruit: FruitConstants.Apple,
    state: 'Georgia',
  },
  format: {
  },
  allowSort: {
    code: true,
    description: false,
    isActive: true,
    myFruit: false,
    state: true,
  },
  displayName: {
    code: 'Code',
    description: 'My customer header',
    isActive: 'IsActive',
    myFruit: 'MyFruit',
    state: 'State of residence',
  },
  description: {
    code: 'The widget code',
    description: 'A tooltip for description',
    isActive: '',
    myFruit: '',
    state: 'What state is this thing from?',
  },
}
