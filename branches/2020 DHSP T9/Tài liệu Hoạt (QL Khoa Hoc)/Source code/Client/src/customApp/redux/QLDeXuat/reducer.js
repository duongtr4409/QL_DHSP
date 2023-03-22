/*
 *
 * PhanQuyen reducer
 *
 */

import { fromJS } from "immutable";
import * as constants from "./constants";

export const initialState = {
  listDeXuat: null,
  chiTietDeXuat: null,
  loading: false,
  success: null,
  TotalRow: 0,
};

function QLDeXuatReducer(state = initialState, action) {
  switch (action.type) {
    case constants.GET_DEXUAT_ACTION:
      return {
        ...state,
        loading: true,
        success: null,
        TotalRow: 0,
        listDeXuat: [],
      };

    case constants.GET_DEXUAT_ACTION_SUCCESS:
      return {
        ...state,
        loading: false,
        success: true,
        TotalRow: action.payload.TotalRow,
        listDeXuat: action.payload.listDeXuat,
      };
    // return state.set("listDeXuat", action.payload.listDeXuat).set("loading", false).set("TotalRow", action.payload.TotalRow);
    case constants.GET_DEXUAT_ACTION_ERROR:
      return {
        ...state,
        success: false,
        loading: false,
        TotalRow: 0,
        listDeXuat: [],
        error: action.payload,
      };
    case constants.GET_DEXUAT_CHITIET_ACTION:
      return {
        ...state,
        loading: true,
        success: null,
        chiTietDeXuat: null,
      };

    case constants.GET_DEXUAT_CHITIET_SUCCESS:
      return {
        ...state,
        loading: false,
        success: true,

        chiTietDeXuat: action.payload.chiTietDeXuat,
      };
    // return state.set("listDeXuat", action.payload.listDeXuat).set("loading", false).set("TotalRow", action.payload.TotalRow);
    case constants.GET_DEXUAT_CHITIET_ERROR:
      return {
        ...state,
        success: false,
        loading: false,

        error: action.payload,
      };
    default:
      return state;
  }
}

export default QLDeXuatReducer;
