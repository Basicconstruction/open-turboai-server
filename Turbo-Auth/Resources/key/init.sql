# key
create table Models
(
    ModelId int auto_increment
        primary key,
    Name    varchar(30) not null
);

create table SupplierKeys
(
    SupplierKeyId int auto_increment
        primary key,
    BaseUrl       varchar(200) not null,
    ApiKey        varchar(200) not null,
    RequestIdentifier int not null
);

create table ModelFees
(
    ModelFeeId    int auto_increment
        primary key,
    SupplierKeyId int    not null,
    ModelId       int    not null,
    Fee           double not null,
    constraint FK_ModelFees_Models_ModelId
        foreign key (ModelId) references Models (ModelId)
            on delete cascade,
    constraint FK_ModelFees_SupplierKeys_SupplierKeyId
        foreign key (SupplierKeyId) references SupplierKeys (SupplierKeyId)
            on delete cascade
);

create index IX_ModelFees_ModelId
    on ModelFees (ModelId);

create index IX_ModelFees_SupplierKeyId
    on ModelFees (SupplierKeyId);

