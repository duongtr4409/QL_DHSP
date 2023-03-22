module.exports = Object.freeze({
  TINH: "1",
  HUYEN: "2",
  XA: "3",

  MODAL_LARGE: 767,
  MODAL_NORMAL: 600,
  MODAL_SMALL: 416,
  ITEM_LAYOUT_MODAL_LARGE: {
    labelAlign: "left",
    labelCol: {span: 6},
    wrapperCol: {span: 18},
  },
  ITEM_LAYOUT: {
    labelAlign: "left",
    labelCol: {span: 3},
    wrapperCol: {span: 21},
  },
  ITEM_LAYOUT2: {
    labelAlign: "left",
    labelCol: {lg: 4, xs: 4},
    wrapperCol: {lg: 20, xs: 20},
  },
  ITEM_LAYOUT3: {
    labelAlign: "left",
    labelCol: {lg: 7, xs: 7},
    wrapperCol: {lg: 16, xs: 16},
  },
  ITEM_LAYOUT_HALF: {
    labelAlign: "left",
    labelCol: {lg: 4, xs: 4},
    wrapperCol: {lg: 16, xs: 16},
  },
  ITEM_LAYOUT_HALF2: {
    labelAlign: "left",
    labelCol: {lg: 8, xs: 8},
    wrapperCol: {lg: 14, xs: 14},
  },
  ITEM_LAYOUT_HALF3: {
    labelAlign: "left",
    labelCol: {lg: 14, xs: 14},
    wrapperCol: {lg: 9, xs: 9},
  },
  COL_ITEM_LAYOUT_HALF: {
    xs: {span: 24},
    lg: {span: 12}
  },
  COL_ITEM_LAYOUT_HALF2: {
    xs: {span: 24},
    lg: {span: 12}
  },
  COL_COL_ITEM_LAYOUT_RIGHT: {
    xs: {span: 24},
    lg: {span: 24, offset: 2}
  },
  COL_COL_ITEM_LAYOUT_RIGHT2: {
    xs: {span: 24},
    lg: {span: 24, offset: 2}
  },
  REQUIRED: {
    required: true,
    message: "Thông tin bắt buộc"
  },

  API_ERROR: {
    title: 'Không thể cập nhật',
    content: 'Đã có lỗi xảy ra ...'
  },

  fileUploadLimit: 10, //MB

  STYLE: {
    tableToXls: {border: 'none', fontSize: '14pt', fontFamily: 'Times New Roman'},
    tableToXls_td: {border: 'none'},
    tableToXls_tableTd: {border: '1px solid #333'},
    tableToXls_provincial: {verticalAlign: "top", display: "inline-block", float: "left", textAlign: "center"},
    tableToXls_country: {display: "inline-block", float: "right", textAlign: "center"},
    tableToXls_title: {textAlign: "center"},
    tableToXls_sign: {display: "inline-block", float: "right", textAlign: "center", width: 300, paddingBottom: 40},

    tableData: {border: '1px solid #333', fontSize: '13pt', fontFamily: 'Times New Roman'},
    tableData_td: {border: '1px solid #333'},
    tableData_th: {border: '1px solid #333'},
  },
  STYLE2: {
    tableToXls: {border: 'none'},
    tableToXls_td: {border: 'none'},
    tableToXls_tableTd: {border: 'none'},
    tableToXls_provincial: {verticalAlign: "top", display: "inline-block", float: "left", textAlign: "center"},
    tableToXls_country: {display: "inline-block", float: "right", textAlign: "center"},
    tableToXls_title: {textAlign: "center"},
    tableToXls_sign: {display: "inline-block", float: "right", textAlign: "center", width: 300, paddingBottom: 40},

    tableData: {},
    tableData_td: {},
    tableData_th: {},
  },
  COMPUTER_SIZE: 1200,

  LoaiThongTinNhaKhoaHoc: {
    QuaTrinhDaoTao: 1,
    QuaTrinhCongTac: 2,
    NgoaiNgu: 3,
    VanBangChungChi: 4,
    GiaiThuongKhoaHoc: 5,
    DuAnDeTai: 6,
    BaiBaoTapChi: 7,
    KetQuaNghienCuu: 8,
    SachChuyenKhao: 9,
    CacMonGiangDay: 10,
    HuongNghienCuuChinh: 11,
    BaoCaoKhoaHoc: 12,
    Url: 13,
    SanPhamDaoTao: 14,
    SanPhamKhac: 15
  },

  LoaiFileDinhKem: {
    DanhMucBieuMau: 1,
    DeXuatDeTai: 2,
    Khac: 3,
    DeTai: 4,
    DuyetDeXuat: 5,
    LyLich: 6,
    AnhDaiDien: 7,
    SanPhamDeTai: 8,
    KetQuaChuyenGiao: 9,
    KetQuaNghienCuu: 10,
    DanhGiaGiaiDoan: 11,
    KetQuaDanhGia: 12,
    BaiBaoTapChi: 13,
    KetQuaNghienCuuCongBo: 14,
    SachChuyenKhao: 15,
    CacMonGiangDay: 16,
    HoatDongKhoaHoc: 17,
    ThuyetMinhDeTai: 18,
    ThongBao: 19
  },
});