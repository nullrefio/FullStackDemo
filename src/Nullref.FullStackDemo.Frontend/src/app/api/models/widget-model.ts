/* tslint:disable */
/* eslint-disable */
import { FruitConstants } from './fruit-constants';
export interface WidgetModel {

  /**
   * The widget code
   */
  code: string;

  /**
   * A tooltip for description
   */
  description: string;
  id: string;
  isActive: boolean;
  myFruit: FruitConstants;

  /**
   * What state is this thing from?
   */
  state: string;
}

export const WidgetModelMetadata = {
  identifier: {
    code: 'code',
    description: 'description',
    id: 'id',
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
    id: true,
    isActive: false,
    myFruit: false,
    state: false,
  },
  required: {
    code: true,
    description: true,
    id: true,
    isActive: true,
    myFruit: true,
    state: true,
  },
  nullable: {
    code: false,
    description: false,
    id: false,
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
    id: 'uuid',
  },
  allowSort: {
    code: true,
    description: false,
    id: false,
    isActive: true,
    myFruit: false,
    state: true,
  },
  displayName: {
    code: 'Code',
    description: 'My custom header',
    id: 'Id',
    isActive: 'IsActive',
    myFruit: 'MyFruit',
    state: 'State of residence',
  },
  description: {
    code: 'The widget code',
    description: 'A tooltip for description',
    id: '',
    isActive: '',
    myFruit: '',
    state: 'What state is this thing from?',
  },
}
