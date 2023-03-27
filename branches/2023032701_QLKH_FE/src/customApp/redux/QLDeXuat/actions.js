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
    type: constants.GET_DEXUAT_ACTION,
    payload: { filterData },
  };
}
export function getDeXuatChiTiet(DeTaiID) {
  return {
    type: constants.GET_DEXUAT_CHITIET_ACTION,
    payload: { DeTaiID },
  };
}

// export function getListSuccess(payload) {
//   return {
//     type: constants.GET_DEXUAT_ACTION_SUCCESS,
//     payload,
//   };
// }

// export function getListFail(error) {
//   return {
//     type: constants.GET_DEXUAT_ACTION_ERROR,
//     error,
//   };
// }
