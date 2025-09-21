use teste

select * from expocambial.TB_DEPARTAMENTO

select * from expocambial.TB_USUARIO

select * from expocambial.TB_EXPO_CAMBIAL

--update expocambial.TB_EXPO_CAMBIAL
--set HOUVE_EXPOSICAO = 1
--where MES_REFERENCIA = '2025-08-01' and ID_DEPARTAMENTO = 1

insert into expocambial.TB_EXPO_CAMBIAL VALUES ('2025-08-01', GetDate(), 0, 1, 1, null, null);

select b.NOME_DEPARTAMENTO from expocambial.TB_USUARIO as a join expocambial.TB_DEPARTAMENTO as b on a.ID_DEPARTAMENTO = b.ID_DEPARTAMENTO
where a.EMAIL = 'italopenha77@outlook.com'

select a.MES_REFERENCIA, a.DT_ENVIO, a.HOUVE_EXPOSICAO, b.NOME_DEPARTAMENTO, b.JUNCAO, c.NOME_USUARIO, c.EMAIL 
from expocambial.TB_EXPO_CAMBIAL as a 
join expocambial.TB_DEPARTAMENTO as b on a.ID_DEPARTAMENTO = b.ID_DEPARTAMENTO
join expocambial.TB_USUARIO as c on c.ID_USUARIO = a.ID_USUARIO
where a.MES_REFERENCIA = '2025-08-01' and b.NOME_DEPARTAMENTO = 'Financeiro'

