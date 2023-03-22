/*
 *
 * PhanQuyen actions
 *
 */

import * as constants from "./constants";

export function defaultAction() {
  return {
    type: constants.DEFAULT_ACTION,
  };
}
export function getList(filterData) {
  return {
    type: constants.GET_DETAI_ACTION,
    payload: { filterData },
  };
}
