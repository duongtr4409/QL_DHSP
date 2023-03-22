//Important Note:
// - Menu không thuộc dạng cha - con thì không có field children hoặc children = null
const options = [
  {
    key: "don-vi-nghien-cuu",
    label: "Đơn vị nghiên cứu",
    leftIcon: "profile",
    showMenu: true,
  },
  {
    key: "ly-lich-khoa-hoc",
    label: "Lý lịch khoa học",
    leftIcon: "usergroup-add",
    showMenu: true,
  },
  {
    key: "ql-nha-khoa-hoc",
    label: "Quản lý nhà khoa học",
    leftIcon: "usergroup-add",
    showMenu: true,
  },
  {
    key: "chi-tiet-nha-khoa-hoc",
    label: "Lý lịch khoa học",
    leftIcon: "user",
    showMenu: true,
  },
  {
    key: "ql-de-xuat",
    label: "Đề xuất đề tài",
    leftIcon: "appstore",
    showMenu: true,
  },
  {
    key: "thuyet-minh-de-tai",
    label: "Thuyết minh đề tài",
    leftIcon: "file-text",
    showMenu: true,
  },
  {
    key: "ql-de-tai",
    label: "Nhiệm vụ nghiên cứu",
    leftIcon: "snippets",
    showMenu: true,
  },
  {
    key: "ql-hoi-dong",
    label: "Hội đồng xét duyệt",
    leftIcon: "usergroup-add",
    showMenu: true,
  },
  {
    key: "bao-cao",
    label: "Báo cáo, thống kê",
    leftIcon: "pie-chart",
    showMenu: true,
  },
  {
    key: "ql-bieu-mau",
    label: "Quản lý biểu mẫu",
    leftIcon: "edit",
    showMenu: true,
  },
  {
    key: "ql-thong-bao",
    label: "Quản lý thông báo",
    leftIcon: "bell",
    showMenu: true,
  },
 
  // {
  //   key: "ht-tai-khoan",
  //   label: "Quản lý tài khoản",
  //   leftIcon: "setting",
  //   showMenu: true,
  // },
  // {
  //   key: "tham-so",
  //   label: "Tham số hệ thống",
  //   leftIcon: "setting",
  //   showMenu: true,
  // },
  // {
  //   key: "phan-quyen",
  //   label: "Quản lý phân quyền",
  //   leftIcon: "setting",
  //   showMenu: true,
  // },
  // {
  //   key: "buoc-thuc-hien",
  //   label: "Danh mục bước thực hiện",
  //   leftIcon: "setting",
  //   showMenu: true,
  // },
  // {
  //   key: "dm-loai-ket-qua",
  //   label: "Danh mục loại kết quả",
  //   leftIcon: "setting",
  //   showMenu: true,
  // },
  // {
  //   key: "dm-loai-hinh-nghien-cuu",
  //   label: "Loại hình nghiên cứu",
  //   leftIcon: "setting",
  //   showMenu: true,
  // },
  // {
  //   key: "dm-cap-de-tai",
  //   label: "Danh mục cấp đề tài",
  //   leftIcon: "setting",
  //   showMenu: true,
  // },
  // {
  //   key: "dm-linh-vuc",
  //   label: "Danh mục lĩnh vực",
  //   leftIcon: "setting",
  //   showMenu: true,
  // },
  {
    key: "quan-tri-he-thong",
    label: "Quản trị hệ thống",
    leftIcon: "setting",
    showMenu: true,
    children: [
      // {
      //   key: "don-vi-nghien-cuu",
      //   label: "Đơn vị nghiên cứu",
      //   leftIcon: "profile",
      //   showMenu: true,
      // },
      {
        key: "ht-tai-khoan",
        label: "Quản lý tài khoản",
        leftIcon: "profile",
      },
      {
        key: "tham-so",
        label: "Tham số hệ thống",
        leftIcon: "profile",
      },
      {
        key: "phan-quyen",
        label: "Quản lý phân quyền",
        leftIcon: "profile",
      },
      {
        key: "buoc-thuc-hien",
        label: "Danh mục bước thực hiện",
        leftIcon: "profile",
        showMenu: true,
      },
      // {
      //   key: "dm-trang-thai",
      //   label: "Danh mục trạng thái",
      // },
      {
        key: "dm-loai-ket-qua",
        label: "Danh mục loại kết quả",
        leftIcon: "profile",
      },
      {
        key: "dm-loai-hinh-nghien-cuu",
        label: "Danh mục loại hình nghiên cứu",
        leftIcon: "profile",
      },
      {
        key: "dm-cap-de-tai",
        label: "Danh mục cấp đề tài",
        leftIcon: "profile",
      },
      {
        key: "dm-linh-vuc",
        label: "Danh mục lĩnh vực",
        leftIcon: "profile",
      },
    ],
  },
];
export default options;
