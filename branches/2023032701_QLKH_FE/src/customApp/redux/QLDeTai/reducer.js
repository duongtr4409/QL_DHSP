/*
 *
 * PhanQuyen reducer
 *
 */

import { fromJS } from "immutable";
import * as constants from "./constants";

export const initialState = {
  listDeTai: null,
  chiTietDeTai: null,
  loading: false,
  success: null,
  TotalRow: 0,
};

function QLDeTaiReducer(state = initialState, action) {
  switch (action.type) {
    case constants.GET_DETAI_ACTION:
      return {
        ...state,
        loading: true,
        success: null,
        TotalRow: 0,
        listDeTai: [],
      };

    case constants.GET_DETAI_ACTION_SUCCESS:
      return {
        ...state,
        loading: false,
        success: true,
        TotalRow: action.payload.TotalRow,
        listDeTai: action.payload.listDeTai,
      };
    // return state.set("listDeTai", action.payload.listDeTai).set("loading", false).set("TotalRow", action.payload.TotalRow);
    case constants.GET_DETAI_ACTION_ERROR:
      return {
        ...state,
        success: false,
        loading: false,
        TotalRow: 0,
        listDeTai: [],
        error: action.payload,
      };
    case constants.GET_DETAI_CHITIET_ACTION:
      return {
        ...state,
        loading: true,
        success: null,
        chiTietDeTai: null,
      };

    case constants.GET_DETAI_CHITIET_SUCCESS:
      return {
        ...state,
        loading: false,
        success: true,

        chiTietDeTai: action.payload.chiTietDeTai,
      };
    // return state.set("listDeTai", action.payload.listDeTai).set("loading", false).set("TotalRow", action.payload.TotalRow);
    case constants.GET_DETAI_CHITIET_ERROR:
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

export default QLDeTaiReducer;
