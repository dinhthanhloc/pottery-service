-- Tạo database nếu chưa có (chạy bằng psql)
SELECT 'CREATE DATABASE "test-pv"'
WHERE NOT EXISTS (
    SELECT FROM pg_database WHERE datname = 'test-pv'
)\gexec

-- Kết nối vào database test-pv rồi chạy phần dưới
-- \c test-pv

-- =========================================
-- 1. Danh mục loại sản phẩm
-- =========================================
CREATE TABLE categories (
    id              BIGSERIAL PRIMARY KEY,
    name            VARCHAR(100) NOT NULL UNIQUE,
    description     VARCHAR(500),
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at      TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- =========================================
-- 2. Sản phẩm
-- =========================================
CREATE TABLE products (
    id              BIGSERIAL PRIMARY KEY,
    category_id     BIGINT NOT NULL,
    name            VARCHAR(150) NOT NULL,
    sku             VARCHAR(50) UNIQUE,
    description     VARCHAR(500),
    current_price   NUMERIC(18,2) NOT NULL CHECK (current_price >= 0),
    is_active       BOOLEAN NOT NULL DEFAULT TRUE,
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_products_category
        FOREIGN KEY (category_id) REFERENCES categories(id),

    CONSTRAINT uq_products_name UNIQUE (name)
);

-- =========================================
-- 3. Lịch sử giá sản phẩm
-- =========================================
CREATE TABLE product_price_histories (
    id              BIGSERIAL PRIMARY KEY,
    product_id      BIGINT NOT NULL,
    price           NUMERIC(18,2) NOT NULL CHECK (price >= 0),
    valid_from      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    note            VARCHAR(255),

    CONSTRAINT fk_price_history_product
        FOREIGN KEY (product_id) REFERENCES products(id)
        ON DELETE CASCADE
);

-- =========================================
-- 4. Hóa đơn bán hàng
-- =========================================
CREATE TABLE sales (
    id              BIGSERIAL PRIMARY KEY,
    sale_code       VARCHAR(50) NOT NULL UNIQUE,
    sale_date       TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    total_amount    NUMERIC(18,2) NOT NULL DEFAULT 0 CHECK (total_amount >= 0),
    customer_name   VARCHAR(150),
    note            VARCHAR(500),
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- =========================================
-- 5. Chi tiết hóa đơn
-- unit_price là giá tại thời điểm bán
-- => đảm bảo lưu được lịch sử doanh thu chính xác
-- =========================================
CREATE TABLE sale_items (
    id              BIGSERIAL PRIMARY KEY,
    sale_id         BIGINT NOT NULL,
    product_id      BIGINT NOT NULL,
    quantity        INT NOT NULL CHECK (quantity > 0),
    unit_price      NUMERIC(18,2) NOT NULL CHECK (unit_price >= 0),
    line_total      NUMERIC(18,2) NOT NULL CHECK (line_total >= 0),

    CONSTRAINT fk_sale_items_sale
        FOREIGN KEY (sale_id) REFERENCES sales(id)
        ON DELETE CASCADE,

    CONSTRAINT fk_sale_items_product
        FOREIGN KEY (product_id) REFERENCES products(id),

    CONSTRAINT uq_sale_product UNIQUE (sale_id, product_id)
);

-- =========================================
-- 6. Index phục vụ thống kê / tra cứu
-- =========================================
CREATE INDEX idx_products_category_id ON products(category_id);
CREATE INDEX idx_sales_sale_date ON sales(sale_date);
CREATE INDEX idx_sale_items_sale_id ON sale_items(sale_id);
CREATE INDEX idx_sale_items_product_id ON sale_items(product_id);